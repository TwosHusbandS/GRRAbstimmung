<!--
Shamelessly stolen from: https://github.com/othneildrew/Best-README-Template
-->

<!--
*** Thanks for checking out the Best-README-Template. If you have a suggestion
*** that would make this better, please fork the repo and create a pull request
*** or simply open an issue with the tag "enhancement".
*** Thanks again! Now go create something AMAZING! :D
-->

<!-- PROJECT SHIELDS -->
<!--
*** I'm using markdown "reference style" links for readability.
*** Reference links are enclosed in brackets [ ] instead of parentheses ( ).
*** See the bottom of this document for the declaration of the reference variables
*** for contributors-url, forks-url, etc. This is an optional, concise syntax you may use.
*** https://www.markdownguide.org/basic-syntax/#reference-style-links
-->

[![Discord][discord-shield]][discord-url]
[![Twitter][twitter-shield]][twitter-url]
[![MIT License][license-shield]][license-url]
[![Maintained][maintained-shield]][maintained-url]

<!-- PROJECT LOGO -->
<br />
<p align="center">
  <!--<a href="https://github.com/TwosHusbandS/DasIstRaueberMusik">
    <img src="DIRM/Artwork/icon.png" alt="Logo" width="80" height="80">
  </a> -->

  <h3 align="center">GRR Abstimmung</h3>

  <p align="center">
    Gets TopLevelComments and Replies from the Einreichungs-Post and posts them in the Abstimmung-Posts /r/GermanRapReddit subreddit
    <br />
    <a href="https://www.youtube.com/watch?v=dQw4w9WgXcQ&t=PLACEHOLDER">View Demo</a>
	.
    <a href="#contact">Contact me</a>
  </p>
</p>



-----

<!-- ABOUT THE PROJECT -->
## About The Project

Gets TopLevelComments and Replies from the Einreichungs-Post and posts them in the Abstimmung-Posts /r/GermanRapReddit subreddit

-----

## Main Features

* Gets lots of information from config file

-----

## Options

### Config Possibilities:
* See [example config file](https://github.com/TwosHusbandS/GRRAbstimmung/blob/master/ConfigFiles/config.ini.example)
* REDDIT_CLIENT_ID (string)
* REDDIT_CLIENT_SECRET (string)
* REDDIT_ACCESS_TOKEN (not used atm)
* REDDIT_REFRESH_TOKEN (string)
* COPYCOMMENTSFROM (string, link from the posts we copy from)
* KARMAREQUIRED (int, required Karma for Einreichung to be considered. Needs to pass this check 3 times (Reddit returns slightly incorrect Karma.) Defaults to 0)
* DELAY (int, Delay in MS between Posts. Default 11000. Needed to not get rate-limited))
* COPYCOMMENTSFROM (string, link from the posts we copy to)

-----

## Built With

* Pretty much built with straight C#, dotnet 6.0, multiplatform
* Did some small hackaround inside the project to get the ConfigFiles to copy to the correct directory. Unloud project to see. Prolly not best practice.
* Shoutout to [Reddit.NET](https://github.com/sirkris/Reddit.NET)

-----


## Contributing

* Feel free to PR, but I guess this is done


-----

## License

Distributed under the MIT License. See `LICENSE` for more information.

As long as you dont copy this 1:1 and charge money for it, we gucci.



-----

## Contact

Twitter - [@thsBizz][twitter-url]

Project Link - [github.com/TwosHusbandS/GRRWeeklyThread][grrweekly-url]

Discord - [@ths#0305][discord-url]


<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->
[discord-url]: https://discordapp.com/users/612259615291342861
[twitter-url]: https://twitter.com/thSbizz
[grrweekly-url]: https://github.com/TwosHusbandS/GRRWeeklyThread
[twitter-shield]: https://img.shields.io/badge/Twitter-@thSbizz-1DA1F2?style=plastic&logo=Twitter
[discord-shield]: https://img.shields.io/badge/Discord-@thS%230305-7289DA?style=plastic&logo=Discord
[license-shield]: https://img.shields.io/badge/License-MIT-4DC71F?style=plastic
[license-url]: https://github.com/TwosHusbandS/GRRWeeklyThread/blob/master/LICENSE.md
[maintained-shield]: https://img.shields.io/badge/Maintained-Meh-FFDB3A?style=plastic
[maintained-url]: #Contributing






