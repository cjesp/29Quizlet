# 29Quizlet
A Quizlet clone for Windows Phone (UWP). This was a project for investigating the UWP platform and has been released to the Microsoft store. It uses the [Template10](https://github.com/Windows-XAML/Template10) library. Tested with the Win 10 15063 SDK.

# How to compile and run this project
* Setup a dev account at [Quizlet](https://quizlet.com/api/2.0/docs)
* Either have a physical Windows Phone or Windows Phone emulator (see [requirements](https://docs.microsoft.com/en-us/windows/uwp/debug-test-perf/test-with-the-emulator#system-requirements))

With the Quizlet dev account details after signup, fill out the token as well client id in:
* LoginPageViewModel
* LoginSignupPageViewModel

e.g. update client_id in LoginSignupPageViewModel:
```
  var guid = Guid.NewGuid().ToString();
  var uri = new Uri($"https://quizlet.com/authorize?response_type=code&client_id=XXXXX&scope=read%20write_set%20write_group&state={guid}");
  Task.Run(() => Launcher.LaunchUriAsync(uri));
```
and you should be good to go!

For a walktrough of the apps features check out this video:
[![Video](https://img.youtube.com/vi/g87Ysq1cIE8/maxresdefault.jpg)](https://youtu.be/g87Ysq1cIE8)



