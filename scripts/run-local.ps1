#Requires -Version 5.1
<#
.SYNOPSIS
    Arranca el minicurso en local sin fallar cuando el puerto ya está ocupado.

.DESCRIPTION
    - Si el puerto lo ocupa un devserver viejo de ESTE proyecto, lo cierra.
    - Si lo ocupa otro programa, busca el siguiente puerto libre.
    - Solo libera el puerto (sin arrancar la app) con -FreeOnly.
    - No abre el navegador con -NoBrowser (lo usa la tarea serve de F5).

.EXAMPLE
    powershell -ExecutionPolicy Bypass -File scripts/run-local.ps1

.EXAMPLE
    powershell -ExecutionPolicy Bypass -File scripts/run-local.ps1 -Port 5200 -NoBrowser
#>
param(
    [int]$Port = 5101,

    # Usado por la tarea "serve" de VS Code antes de depurar (F5).
    [switch]$NoBrowser,

    # Solo libera el puerto y termina (diagnóstico).
    [switch]$FreeOnly
)

$ErrorActionPreference = 'Stop'

$raizProyecto = Split-Path -Parent $PSScriptRoot
$proyecto = Join-Path $raizProyecto 'MinicursoCalculoII.csproj'

if (-not (Test-Path $proyecto)) {
    throw "No se encontró el proyecto en $proyecto"
}

function Get-DuenoDelPuerto([int]$puerto) {
    try {
        Get-NetTCPConnection -State Listen -LocalPort $puerto -ErrorAction SilentlyContinue |
            Select-Object -First 1 -ExpandProperty OwningProcess
    }
    catch {
        $null
    }
}

function Test-DevServerDeEsteProyecto([int]$procesoId) {
    if (-not $procesoId) { return $false }

    $proceso = Get-CimInstance Win32_Process -Filter "ProcessId=$procesoId" -ErrorAction SilentlyContinue
    if (-not $proceso) { return $false }

    # El devserver de Blazor corre como dotnet + blazor-devserver.dll
    return ($proceso.Name -eq 'dotnet.exe') -and
           ($proceso.CommandLine -match 'blazor-devserver|MinicursoCalculoII')
}

function Liberar-Puerto([int]$puerto) {
    $dueno = Get-DuenoDelPuerto $puerto
    if (-not $dueno) { return $true }

    if (Test-DevServerDeEsteProyecto $dueno) {
        Write-Host "El puerto $puerto lo ocupa un servidor viejo de este proyecto (PID $dueno). Cerrandolo..." -ForegroundColor Yellow
        Stop-Process -Id $dueno -Force -ErrorAction SilentlyContinue
        Start-Sleep -Milliseconds 900
        return -not (Get-DuenoDelPuerto $puerto)
    }

    Write-Host "El puerto $puerto lo ocupa otro programa (PID $dueno)." -ForegroundColor Yellow
    return $false
}

function Abrir-Navegador([int]$puerto) {
    Start-Process powershell -WindowStyle Hidden -ArgumentList @(
        '-NoProfile', '-Command',
        "Start-Sleep -Seconds 4; Start-Process 'http://localhost:$puerto'"
    ) | Out-Null
}

if (Liberar-Puerto $Port) {
    if ($FreeOnly) { exit 0 }
}
elseif ($FreeOnly) {
    Write-Host "No se pudo liberar el puerto $Port. Cierra el programa que lo usa antes de depurar." -ForegroundColor Red
    exit 1
}
else {
    # El puerto está ocupado por algo ajeno: usar el siguiente puerto libre.
    $puertoLibre = $Port + 1
    while ($puertoLibre -lt ($Port + 40) -and (Get-DuenoDelPuerto $puertoLibre)) {
        $puertoLibre++
    }

    if (Get-DuenoDelPuerto $puertoLibre) {
        throw "No hay puertos libres entre $Port y $($Port + 39)."
    }

    Write-Host "Usando el puerto alternativo http://localhost:$puertoLibre" -ForegroundColor Cyan
    $Port = $puertoLibre
}

Write-Host "Iniciando en http://localhost:$Port" -ForegroundColor Cyan

if (-not $NoBrowser) {
    Abrir-Navegador $Port
}

$env:ASPNETCORE_ENVIRONMENT = 'Development'
dotnet run --project $proyecto --no-launch-profile --urls "http://localhost:$Port"
exit $LASTEXITCODE
