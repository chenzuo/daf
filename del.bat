attrib *.* /s  -r -s -h
rd TestResults /s/q
del *.testrunconfig /s/f/q
del *.scc /s/f/q
del *.vspscc /s/f/q
del *.vssscc /s/f/q
del *.cache /s/f/q

for /f "tokens=*" %%a in ('dir "bin" /b /ad /s') do rd "%%a" /q/s
for /f "tokens=*" %%b in ('dir "obj" /b /ad /s') do rd "%%b" /q/s

