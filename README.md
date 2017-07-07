# WebSqlLang
<p> This project is here to create free alternative for a web parser / automation tool for people that doesn't know any programming languages but can wrote SQL querys for their databases or for any other reason. SQL languages are simple to use and easy to learn for majority of people. The goal is to create an up that gets some SQL query as input and result in a table, csv or JSON output to window or file. </p>

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
