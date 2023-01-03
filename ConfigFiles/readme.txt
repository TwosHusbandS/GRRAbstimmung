Config Possibilities:
* REDDIT_CLIENT_ID 
* REDDIT_CLIENT_SECRET 
* REDDIT_ACCESS_TOKEN 
* REDDIT_REFRESH_TOKEN 
* COPYCOMMENTSFROM 
* KARMAREQUIRED (required Karma for Einreichung to be considered. Needs to pass this check 3 times (Reddit returns slightly incorrect Karma.) Defaults to 0)
* DELAY (Delay in MS between Posts. Default 11000. Needed to not get rate-limited))
* COPYCOMMENTSFROM

Workflow:
- Change COPYCOMMENTSFROM in the config.ini to the link from the Einreichungs Thread
- Post Abstimmungsthread.
- Remove Abstimmungsthread so its not public.
- Change COPYCOMMENTSFROM in the config.ini to the link from the Abstimmungs Thread you just posted
- Run the program and wait. It will take a while.
- Check the thread to see if it did what its supposed to.
- Approve Abstimmungsthread so its public.
- Distinguish / sticky