/* Copyright © 2017 Mykhailo Tsvietukhin. This program is released under the "GPL-3.0" lisence. Please see LICENSE for license terms. */

# WebSqlLang
<p> This project is here to create free alternative for a web parser / automation tool for people that doesn't know any programming languages but can wrote SQL querys for their databases or for any other reason. <BR> SQL languages are simple to use and easy to learn for majority of people. The goal is to create an up that gets some SQL query as input and result in a table, csv or JSON output to window or file. </p>

### Week 3 Report.

<p>For last 3 weeks I decided a platform and project. Created Mockup of the UI and working on ideas for future realization. Starting learning of method of creation on my own leanguage. Started working on backend implementation. I find out that I need to schedule a specific time to work on a project during the day to be more organized. Next week I plan to work on a UI to make some buttons to work and continue to work on back end to test project from simple recieveing headers to making more interesting things. </p>


### PLATFORM: WINDOWS / LINUX / MONO * (http://www.mono-project.com/)
### Language: C# .NET
### IDE: Visual Studio / Rider * (https://www.jetbrains.com/rider/)

## Example 1:

### This will gives you all headers of google.com responce

```

SELECT [ALL] using HEADERS FROM http://google.com 

```
## Output 1:

```
NAME            VALUE
host            google.com
cookie          ""

```
## Example 2:

### This will select 2 columns url and text of <a> tag for all urls found on a page

```

SELECT [URL, NAME] using LINKS FROM http://google.com 

```
## Output 1:

```
URL             NAME
goolge.com      goolge

```
## MockUpUI:

![alt text](https://github.com/tsviet/WebSqlLang/blob/master/MockupUI.png)
