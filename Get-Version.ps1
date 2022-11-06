$Content = Get-Content ./RELEASE_VERSION
$Tokens = $Content.Split(' ')
$ReleaseVersion = $Tokens[0]
if ($Tokens.Count.Equals(2)) {
    Add-Content -Path $env:GITHUB_ENV -Value "PreRelease=true"
} else {
    Add-Content -Path $env:GITHUB_ENV -Value "PreRelease=false"
}
Add-Content -Path $env:GITHUB_ENV -Value "ReleaseVersion=${ReleaseVersion}"

# echo $ReleaseVersion
# echo $PreRelease
