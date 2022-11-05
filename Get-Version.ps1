$Content = Get-Content ./RELEASE_VERSION
$Tokens = $Content.Split(' ')
$ReleaseVersion = $Tokens[0]
if ($Tokens.Count.Equals(2)) {
    $PreRelease = 1
} else {
    $PreRelease = 0
}
Add-Content -Path $env:GITHUB_ENV -Value "ReleaseVersion=${ReleaseVersion}"
Add-Content -Path $env:GITHUB_ENV -Value "PreRelease=${PreRelease}"
# echo $ReleaseVersion
# echo $PreRelease
