$excelpad = "C:\Users\Thomas\OneDrive - Hogeschool Gent\rdpping_ips.xlsx"
$iptable = Import-Excel -Path $excelpad
$ip = $iptable.Where({$_.user -eq $env:USERNAME}).ip

#Write-Output $args[1]

if($args[1] -eq "ping"){
    ping $ip
}elseif($args[1] -eq "rdp"){
    Start-Process "$env:windir\system32\mstsc.exe" -ArgumentList "/v:$ip"
}else{
# extra commando
}