#Soner Üzeyir Akar
function Get-Wmi($Class, $Property) {
    try {
        $obj = Get-CimInstance -ClassName $Class -ErrorAction Stop | Select-Object -First 1
        if ($null -ne $obj -and $null -ne $obj.$Property) { return $obj.$Property.ToString().Trim() }
    } catch {
        # fallback to WMI v1 if needed
        try {
            $obj = Get-WmiObject -Class $Class -ErrorAction Stop | Select-Object -First 1
            if ($null -ne $obj -and $null -ne $obj.$Property) { return $obj.$Property.ToString().Trim() }
        } catch {}
    }
    return 'unknown'
}

function Get-HardwareId {
    $cpu  = Get-Wmi 'Win32_Processor' 'ProcessorId'
    $bios = Get-Wmi 'Win32_BIOS' 'SerialNumber'
    $disk = Get-Wmi 'Win32_PhysicalMedia' 'SerialNumber'

    $combined = "$cpu-$bios-$disk"
    $bytes = [System.Text.Encoding]::UTF8.GetBytes($combined)
    $sha = [System.Security.Cryptography.SHA256]::Create()
    $hash = $sha.ComputeHash($bytes)
    # produce uppercase hex string without separators (matches Convert.ToHexString)
    return ([BitConverter]::ToString($hash) -replace '-', '')
}

# run
function Get-Wmi($Class, $Property) {
    try {
        $obj = Get-CimInstance -ClassName $Class -ErrorAction Stop | Select-Object -First 1
        if ($null -ne $obj -and $null -ne $obj.$Property) { return $obj.$Property.ToString().Trim() }
    } catch {
        try {
            $obj = Get-WmiObject -Class $Class -ErrorAction Stop | Select-Object -First 1
            if ($null -ne $obj -and $null -ne $obj.$Property) { return $obj.$Property.ToString().Trim() }
        } catch {}
    }
    return 'unknown'
}

function Get-HardwareId {
    $cpu  = Get-Wmi 'Win32_Processor' 'ProcessorId'
    $bios = Get-Wmi 'Win32_BIOS' 'SerialNumber'
    $disk = Get-Wmi 'Win32_PhysicalMedia' 'SerialNumber'

    $combined = "$cpu-$bios-$disk"
    $bytes = [System.Text.Encoding]::UTF8.GetBytes($combined)
    $sha = [System.Security.Cryptography.SHA256]::Create()
    $hash = $sha.ComputeHash($bytes)
    return ([BitConverter]::ToString($hash) -replace '-', '')
}

# run
Get-HardwareId
