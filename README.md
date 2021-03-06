/* Copyright © 2017 Mykhailo Tsvietukhin. This program is released under the "GPL-3.0" lisence. Please see LICENSE for license terms. */

### Contact information: tsviet@pdx.edu / mtsvetukhin@gmail.com For any suggestion or contribution contact me. 

# WebSqlLang
<p> This project is here to create a free alternative for a web parser/automation tool for people that don't know any programming languages but can write SQL queries for their databases or for any other reason. 
SQL languages are simple to use and easy to learn for the majority of people. The goal is to create an up that gets some SQL query as input and result in a table, CSV or JSON output to window or file. </p>

### PLATFORM: WINDOWS / LINUX (will wait untill .Net Core 2.0 will be released Fall 2017)
### Language: C# .NET
### IDE: Visual Studio / Rider * (https://www.jetbrains.com/rider/) (wait for 2017.2 version before use it. Fall 2017)

### Build instructions

1. Install Visual Studio 2017 Comunity edition (Windows)
2. Clone project 
3. Open project 
4. Start / Build 

## Example 1:

### This will gives you all headers of google.com responce

```

SELECT * 
using HEADERS 
FROM http://google.com 

```
## Output 1:

![alt text](https://github.com/tsviet/WebSqlLang/blob/master/images/example1.png)

## Example 2:

### This will select 2 columns url and text of <a> tag for all urls found on a page

```

SELECT [URL, NAME] 
using LINKS 
FROM http://google.com 

```
## Output 1:

![alt text](https://github.com/tsviet/WebSqlLang/blob/master/images/example2.png)

## This was intended view that I started with...

## MockUpUI:

![alt text](https://github.com/tsviet/WebSqlLang/blob/master/MockupUI.png)

### Week 3 Report.

<p>For last 3 weeks I decided a platform and project. Created Mockup of the UI and working on ideas for future realization. Starting learning of method of creation on my own leanguage. Started working on backend implementation. I find out that I need to schedule a specific time to work on a project during the day to be more organized. Next week I plan to work on a UI to make some buttons to work and continue to work on back end to test project from simple recieveing headers to making more interesting things. </p>
