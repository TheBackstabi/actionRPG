@echo off
robocopy G:\!UnityProjects\ActionRPG\Assets\Data G:\aRPGBuilds\MostRecentBuild\aRPGame_Data\Data /e
robocopy G:\!UnityProjects\ActionRPG\Assets\XML G:\aRPGBuilds\MostRecentBuild\aRPGame_Data\XML /e
cd G:\aRPGBuilds\MostRecentBuild\aRPGame_Data\Data
del /A:H *.META
cd G:\aRPGBuilds\MostRecentBuild\aRPGame_Data\XML
del /A:H *.META
pause