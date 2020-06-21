Auxiliary service has been developped in frame of one of the project I used to work.

The main idea is to provide some auxiliary functions that can be used whenever in the project

****The most important functions****

* ``State mashine`` - allows orchestration of long running processes that are broken down of multiple transaction scopes. It provides an ability to start process and if any fails happen on some of the step, fix errors and resume the process from the save point it reached. 

* ``Notifications`` - the standard way of sending notifications via email, sms etc. It allows specifing multiple templates of messages and apply them in runtime depending on some criteria. 

* ``Scheduler`` - based on ``Quartz`` library. It's aimed to use for any cron tasks. It allows runnign some code in scheduled manner. For the end client it as simple as creating a new class inhereted from one of the provided base class and put execution handler.   
