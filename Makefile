all: main.exe

weather.dll: weather.cs webapi.cs
		mcs -target:library -out:weather.dll weather.cs webapi.cs

main.exe: weather.dll main.cs
		mcs /out:main.exe /reference:weather.dll main.cs
