# فایل: auto-push.ps1

$projectPath = "E:\Prescripto"
$commitMessage = "Auto commit at $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')"

Set-Location $projectPath

git add .
git commit -m "$commitMessage"
git push origin main

