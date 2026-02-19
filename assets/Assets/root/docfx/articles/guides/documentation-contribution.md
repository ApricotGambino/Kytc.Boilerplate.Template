# Document Contribution

If you like reading documentation, you're going to LOVE writing it!

---

Documentation takes time to write, it takes time to read, and worst - it takes time to maintain. 
That's why we're using something called [docfx](https://dotnet.github.io/docfx) to auto-generate our code's documentation.
While this particular documentation is geared towards the developers of the application, it would be trivial to extend
this same structure to provide documentation for use for the application's users. 

## Markdown
Markdown (*.md files) is a standard formatting language and used in nearly every project ever written. 
There's lot's of resources on markdown formatting, like [Markdown Guide](https://www.markdownguide.org/basic-syntax/#overview), and [Github](https://docs.github.com/en/get-started/writing-on-github/getting-started-with-writing-and-formatting-on-github/basic-writing-and-formatting-syntax)

The complications of Markdown comes from knowning what's supported, and what's not, as it depends what you're using to view the .md file. 
For the most part, all markdown renderers support features that Github does, so if it works on Github, it probably works on your renderer. 
For example, language syntax highlighting [here's what GitHub does]("https://docs.github.com/en/get-started/writing-on-github/working-with-advanced-formatting/creating-and-highlighting-code-blocks#syntax-highlighting")

Docfx translates our .md files into HTML, but you can read them just fine on your computer as well, and even within visual studio using an [extension]("https://marketplace.visualstudio.com/items?itemName=MadsKristensen.MarkdownEditor2"). 

## Why use .md for documentation?
Because the alternative is something like .docx, or .pdf.  Both absolutely could work,
but they are inherently difficult to maintain history and aren't very condusive to source control. 

This matters because documentation *should* be relevant to your project, and your project is in source control. 
So as your project changes, so too should your documentation.  

## What does Docfx do?
This documentation is generated through [docfx](https://dotnet.github.io/docfx)

While markdown files are essential to actually creating the documention, Docfx is responsible for bundling all of it up
into one static site that can be hosted easily. 

*like this one...*

In addition, it also automatically parses through your code, and generates documentation for it as well. 

If we only needed documentation without generation, then [Mkdocs]("https://www.mkdocs.org/") would probably be better. 

## How do I add documentation
If you need to add code documentation, or the stuff found in [API](/generated/api/Api.html) then nothing! That's all automagic. 

For everything else, you'll want to follow the structure of how everything else is written, which is generally this: 
```xaml
📂assets                 
  📂root                 
    📂docfx              
        📂images         
        📂build      //Files to build the static doc site
        📂articles   
            📂guides
                📄toc.yml //This tells the site where to find .md files
                📄overview.md    //Overview for the section
                📄documentation-contribution.md  //You are here!
            ...
        📂generated //Generated API stuff. 
        📂templates //Templates where you can find example .md files
    
```
So if you wanted to your own guide, follow these steps: 
1. Copy `~docfx/templates/document-template.md`
2. Paste the copied document into the `~docfx/articles/guides/` directory
3. Rename the file to whatever you want, EX: `my-guide.md`
4. Fill that file with whatever content you want.
5. Edit `~docfx/articles/guides/toc.yml` to this: 
    ```yml
    - name: Overview
      href: overview.md
    - name: Contributing to documentation
      href: documentation-contribution.md
    - name: My Guide    #This is the name in the sidebar.
      href: my-guide.md #This is the file name to be linked to.
    ```

**And that's it!**

But you should probably check your work, to do so, you can execute the build batch file to generate and run a local version of the site. 
> [!IMPORTANT]
> [Node.js]("https://nodejs.org/") is required to run the local version.
>
> Also pay attention to the warnings issued from the build and correct what is reported. (If it's something you actually adjust yourself)

In a powershell prompt: 
```powershell
cd "~docfx\build\"
.\GenerateAndRunLocalDocumentation.bat
```
This batch file will install the required tools to build this documentation, then hosts it on [http://localhost:8080]("http://localhost:8080")
