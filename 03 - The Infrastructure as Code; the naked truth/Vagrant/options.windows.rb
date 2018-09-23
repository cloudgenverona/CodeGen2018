if ! File.exists?('./resources/NDP452-KB2901907-x86-x64-AllOS-ENU.exe')
  puts '.Net 4.5.2 installer could not be found!'
  puts "Please run:n  wget http://download.microsoft.com/download/E/2/1/E21644B5-2DF2-47C2-91BD-63C560427900/NDP452-KB2901907-x86-x64-AllOS-ENU.exe"
  exit 1
end
 
if ! File.exists?('./resources/Octopus.Tentacle.2.6.0.778-x64.msi')
  puts 'Octopus Tentacle installer could not be found!'
  puts "Please run:n  wget http://download.octopusdeploy.com/octopus/Octopus.Tentacle.2.6.0.778-x64.msi"
  exit 1
end