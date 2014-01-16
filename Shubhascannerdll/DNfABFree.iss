
; Set AB_MIN_VERSION to the oldest compatible AmiBroker version
#ifndef AB_MIN_VERSION
  #define AB_MIN_VERSION "5.60.0"
#endif

; Set DNFAB_MIN_VERSION to the oldest compatible DNfAB version
#ifndef DNFAB_MIN_VERSION
  #define DNFAB_MIN_VERSION "5.60.3"
#endif


; -----------------------------------------------------------------------------
; 
; DO NOT CHANGE THE REST OF THE FILE
;
; -----------------------------------------------------------------------------

#define INST_MIN_VERSION "5.6"
#define INST_MAX_VERSION "5.6"

[Dirs]
Name: "{app}\Plugins";
Name: "{app}\.NET for AmiBroker";
Name: "{app}\.NET for AmiBroker\Assemblies";
Name: "{app}\.NET for AmiBroker\Logs"; Permissions: authusers-modify; 

[Files]
;
; AmiBroker folder
;
DestDir: {app}; Source: "{#GetEnv('DotNetForAB_Home')}\.NET for AmiBroker\Tools\Setup\AmiBroker.PlugInHost.dll"; Check: "CheckDNfABVersion('5.6')"; Permissions: authusers-readexec; Flags: IgnoreVersion overwritereadonly uninsremovereadonly replacesameversion; 
;
; AmiBroker\Plugins folder
;
DestDir: {app}\PlugIns; Source: "{#GetEnv('DotNetForAB_Home')}\.NET for AmiBroker\Tools\Setup\DotNetHost.dll"; Check: "CheckDNfABVersion('5.6') AND CheckDNfABPlatform('x86')"; Permissions: authusers-readexec; Flags: IgnoreVersion overwritereadonly uninsremovereadonly replacesameversion; 
DestDir: {app}\PlugIns; Source: "{#GetEnv('DotNetForAB_Home')}\.NET for AmiBroker\Tools\Setup\DotNetHostx64.dll"; Check: "CheckDNfABVersion('5.6') AND CheckDNfABPlatform('x64')"; Permissions: authusers-readexec; Flags: IgnoreVersion overwritereadonly uninsremovereadonly replacesameversion; 
;
; AmiBroker\.NET for AmiBroker\Assemblies folder
;
DestDir: {app}\.NET for AmiBroker\Assemblies; Source: "{#GetEnv('DotNetForAB_Home')}\.NET for AmiBroker\Tools\Setup\AmiBroker.PlugInHost.dll"; Check: "CheckDNfABVersion('5.6')"; Permissions: authusers-readexec; Flags: IgnoreVersion overwritereadonly uninsremovereadonly replacesameversion; 
DestDir: {app}\.NET for AmiBroker\Assemblies; Source: "{#GetEnv('DotNetForAB_Home')}\.NET for AmiBroker\Tools\Setup\AmiBroker.PlugInHost.dll.config"; Check: "CheckDNfABVersion('5.6')"; Permissions: authusers-readexec; Flags: IgnoreVersion overwritereadonly uninsremovereadonly replacesameversion; 
DestDir: {app}\.NET for AmiBroker\Assemblies; Source: "{#GetEnv('DotNetForAB_Home')}\.NET for AmiBroker\Tools\Setup\AmiBroker.Utils.dll"; Check: "CheckDNfABVersion('5.6')"; Permissions: authusers-readexec; Flags: IgnoreVersion overwritereadonly uninsremovereadonly replacesameversion; 

[code]
const

  MinABVersion =  '{#AB_MIN_VERSION}';
  MinDNfABVersion = '{#DNFAB_MIN_VERSION}';
  
  InstallableMinVersion = '{#INST_MIN_VERSION}';
  InstallableMaxVersion = '{#INST_MAX_VERSION}';

  CR = #13;
  LF = #10;
  CRLF = CR + LF;

var

  IsSoftwareChecked    : Boolean;
  
  // Broker.exe
  BrokerFilePath       : String;
  BrokerVersion        : String;
  BrokerCompVersion    : String;
  IsBrokerInstalled    : Boolean;
  IsBrokerVersionOk    : Boolean;    // installed AmiBroker version is ok for the .NET plug-in
  ReqBrokerVersion     : String;
  
  // DotNetHost.dll
  DotNetHostFilePath   : String;
  DotNetHostPlatform   : String;
  DotNetHostVersion    : String;
  DotNetHostCompVersion: String;
  IsDNfABInstalled     : Boolean;
  IsDNfABVersionOk     : Boolean;    // installed DNfAB version is ok for the .NET plug-in
  
  // Check these variables in your code
  CanInstallPlugin     : Boolean;   // Supported AB version is installed, supported DNfAB version is installed or installable -> your .NET plug-in can be installed. 
  CanInstallDNfAB      : Boolean;   // .NET for AB files need to be istalled along with the .NET plug-in

// Checks if AmiBroker is installed and checks if AB version is supported
procedure CheckBrokerFile();
begin
  ReqBrokerVersion := MinABVersion;
  if (CompareText(MinABVersion, Copy(MinDNfABVersion, 0, 3)) < 0) then ReqBrokerVersion := Copy(MinDNfABVersion, 0, 3) + '0';

  BrokerFilePath := ExpandConstant('{app}') + '\Broker.exe';
  IsBrokerInstalled := FileExists(BrokerFilePath);
  if (IsBrokerInstalled) then begin
    GetVersionNumbersString(BrokerFilePath, BrokerVersion);
    BrokerCompVersion := Copy(BrokerVersion, 0, 3);
    IsBrokerVersionOk := CompareText(ReqBrokerVersion, BrokerVersion) <= 0;
  end;
end;

// Checks if .NET for AmiBroker version is installed
procedure CheckDotNetHostFile();
begin
  // Check DotNetHost.dll (x86)
  DotNetHostFilePath := ExpandConstant('{app}') + '\Plugins\DotNetHost.dll';
  DotNetHostPlatform := 'x86';
  IsDNfABInstalled := FileExists(DotNetHostFilePath);
  
  // Check DotNetHostx64.dll (x64)
  if (not IsDNfABInstalled) then begin
    DotNetHostFilePath := ExpandConstant('{app}') + '\Plugins\DotNetHostx64.dll';
    DotNetHostPlatform := 'x64';
    IsDNfABInstalled := FileExists(DotNetHostFilePath);
  end
  
  // If DotNetHost*.dll is found then 
  if (IsDNfABInstalled) then begin
    IsDNfABVersionOk:= GetVersionNumbersString(DotNetHostFilePath, DotNetHostVersion);
    if (IsDNfABVersionOk) then begin
      DotNetHostCompVersion := Copy(DotNetHostVersion, 0,3);
      IsDNfABVersionOk := CompareText(MinDNfABVersion, DotNetHostVersion) <= 0;
    end;
  end
  else begin
    // This will install files for both platforms! // TODO
    DotNetHostPlatform := 'x86;x64;';
  end;
end;

// Called only once, checks if DNfAB Free Edition can be installed
procedure CheckSoftware();
begin

  IsSoftwareChecked := true;
  
  CanInstallDNfAB := false;
  CanInstallPlugin := false;
  
  CheckBrokerFile();

  if (IsBrokerInstalled) then begin
    if (IsBrokerVersionOk)then begin

      CheckDotNetHostFile();

      if (IsDNfABInstalled) then begin
        if (IsDNfABVersionOk) then begin
          if (CompareText(DotNetHostCompVersion, BrokerCompVersion) = 0) then begin
            CanInstallPlugin := True;
          end
          else begin
            MsgBox('The installed .NET for AmiBroker in not compatible with the installed AmiBroker version.', mbCriticalError, MB_OK);
          end;
        end
        else begin
          MsgBox('The installed version of .NET for AmiBroker is v' + DotNetHostVersion + '.' + CRLF + CRLF + 'Please, install v' + MinDNfABVersion + ' or later version manually to make this .NET plugin work!', mbCriticalError, MB_OK);
        end;
      end
      // if (IsDNfABInstalled) - DNfAB is not installed
      else begin
        // if this setup package has Free Edition files for the installed AB version
        if ((CompareText(InstallableMinVersion, BrokerCompVersion) <= 0) AND (CompareText(InstallableMaxVersion, BrokerCompVersion) <= 0)) then begin
          CanInstallDNfAB := true;
          CanInstallPlugin := true;
        end
        // This setup package has no Free Edition files for the installed AB version
        else begin
          MsgBox('Please, install .NET for AmiBroker v' + BrokerCompVersion + 'x manually.', mbCriticalError, MB_OK);
        end;
      end;
    end

    // if (IsBrokerVersionOk) - AB version is older than minimum
    else begin 
      MsgBox('The installed AmiBroker version is not supported. Please, upgrade to v' + ReqBrokerVersion + ' or newer.', mbCriticalError, MB_OK);
    end;
  end
  // if (IsBrokerInstalled) - AB is not installed
  else begin
    MsgBox('AmiBroker is not installed in the target folder.', mbCriticalError, MB_OK);
  end;
end;


procedure CurStepChanged(CurStep: TSetupStep);
begin
  if (CurStep = ssInstall) then begin
    if(NOT IsSoftwareChecked) then CheckSoftware();
    if (not CanInstallPlugin) then Abort();
  end;
end;

// Checks if DNfAB needs to be installed
function CheckDNfAB() : Boolean;
begin
  if(not IsSoftwareChecked) then CheckSoftware();
  
  Result := CanInstallDNfAB;
end;

// Checks if specified version (e.g: '5.4') of DNfAB needs to be installed
function CheckDNfABVersion(Version : String) : Boolean;
begin
  if(not IsSoftwareChecked) then CheckSoftware();
  
	Result := CanInstallDNfAB AND (CompareText(Version, BrokerCompVersion) = 0);
end;

// Checks if specified platform (e.g: 'x86') of DNfAB needs to be installed
function CheckDNfABPlatform(Platform : String) : Boolean;
begin
  if(not IsSoftwareChecked) then CheckSoftware();
  
	Result := CanInstallDNfAB AND (Pos(Platform, DotNetHostPlatform) > 0);
end;

// Checks if the custom .NET plug-in can be installed
function CheckPlugIn() : Boolean;
begin
  if(not IsSoftwareChecked) then CheckSoftware();
 
  Result := CanInstallPlugin;
end;