@ECHO OFF
cd /d %~dp0
for %%a in (%*) do (
ffmpeg -i %%a -r 60 -vsync cfr -af aresample=async=1 -vcodec utvideo -acodec pcm_s16le -colorspace bt709 -pix_fmt yuv422p "%%~dpna.avi"
)
pause