---
title: Hello, InkWell
description: A post that introduces InkWell
updated: 2025-06-07
created: 2025-06-07
published: 2025-06-07
isDraft: false
summary: >
    The summary could be human or AI generated and used
    for several purposes, including text in thumbnails.
---
# Hello, InkWell!

## Purpose
_InkWell_ takes content written in markdown and applies the appropriate
[Mustache](https://mustache.github.io/mustache.5.html) template from the
`html/` directory. The result is a static-HTML site.

## Markdown Content
All markdown content follows a [CommonMark](https://commonmark.org/) format with
front-matter in the header. The data in the front matter becomes available to
the Mustache templates.

## Directory Structure
Directories in the `content/` directory are meant to contain a _family_ of
similar content (e.g., _posts_). _InkWell_ will look for an HTML template in the
`html/templates/ ` directory that matches the name of the _family_. For example
markdown files in the `content/posts/` directory will match to an HTML template
located in `html/templates/posts.html`. That template will be used to render all
the content pages in that directory.

## Front-Matter
Markdown files are processed and their data is available to template when
rendering. Extra tags are ignored. Dates and Date times are parsed as ISO8601
dates and times.

| Key              | Expected Type |
| ---------------- | ------------- |
| Title (required) | String        |
| Desc             | String        |
| Updated          | Date          |
| Created          | Date          |
| Published        | Date          |
| Summary          | String        |
| Author           | AuthorInfo    |

The Author has two properties. Everything is optional.

```csharp
public record AuthorInfo(string Name, string Email);

```

The `index.md` file has some special settings that are available to all pages
when rendering. The front-matter in that file can be thought of as the site's
settings.

## HTML Templates
All HTML templates are [Mustache](https://mustache.github.io/mustache.5.html)
templates. The `public/` directory is copied as is without processing to the
output directory. The site's `index.html` is processed as a template and copied
to the output directory.

## Image Processing
When publishing for the web, images should be resized and optimized to make the
site perform well. Image processing is part of the pipeline.

## Commands
- **compile** The act of processing an InkWell source directory (as shown in the
sample), to an output directory is called "compiling". You issue the command
`inkwell compile path/to/source path/to/output`.


## Key Libraries Used
- **[Spectre Console](https://spectreconsole.net/)** Used to render the TUI and
  process command-line arguments.
- **[Stubble]()** Used to process Mustache templates.
- **[Markdig](https://github.com/xoofx/markdig)** Used to process and render
  Markdown.
- **[YamlDotNet](https://github.com/aaubry/YamlDotNet)** Used to process YAML
- **[Magick.NET](https://github.com/dlemstra/Magick.NET/)** Used to perform
  image processing.
