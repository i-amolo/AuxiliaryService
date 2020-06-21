Auxiliary service has been developped in frame of one of the project I used to work.

The main idea is to provide some auxiliary functions that can be used whenever in the project

****The most important functions****

* ``State mashine`` - allows orchestration of long running process that breaks down of multiple transaction scopes. It provides an ability to start process then fails on some of the step and resulme the process from save point after error has been fixed

* ``Notifications`` - the standard way of sending notifications via email, sms etc. It allows specifing multiple templates of messages and apply them in runtime depending on some criteria. 

* ``Scheduler`` - based on ``Quartz`` library. It's aimed to use for any cron tasks. 