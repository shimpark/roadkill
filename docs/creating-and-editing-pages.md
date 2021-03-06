# Creating and editing pages

## Creating a new page
To create a page in Roadkill, you need to have an editor or administrator login - you cannot edit or create pages anonymously. There are various ways of setting up user rights on your site, which can be found on Users and permissions page.

To create a page, click on the "New page" link on the navigation bar (left side for the media wiki theme). You will then need fill the title for the page and some text content. The format of the text is dictated by the markup type for the site, by default this will be Creole wiki markup which is a more consistent form of the media wiki markup you find on sites such as wikipedia.

The editor is not WYSIWYG, but will insert the tokens/markup for bold, italic, underline, headings, lists, links and images. The question mark button gives you help for the accepted special tokens for the current markup type.

If you are an administrator, you have the option of locking down the page via the "This page can only be edited by administrators." checkbox. This will ensure editor logins cannot edit the page, or revert it to previous versions. This is useful for important pages on your site, such the homepage.

You can embed images into your page using the image button, which launches a file explorer in a new dialog allowing you upload images. By default, Roadkill strips all dangerous HTML from the text which includes script tags, style tags, iframe tags.

You can preview your changes before saving, which displays your page inside a new dialog using the same display engine as the Roadkill site. Once you're happy with this, you can save the page.

Roadkill allows you to categorize your pages using the tags textbox. You can enter relevant tags (which will auto complete if the tag already exists) for the page - more on this below.

## Editing a page
Editor logins and administrators can edit any page in the system, changing the title, tags and text content. This is works the same way (and uses the same interface) as creating a new page.

## Page version history and reverting
Every page edit is stored as a new version in Roadkill, which allows you to revert back to previous versions of the page. This can be done via the "View history" link that is available on every page.

The history page shows every version of the page, clicking on a particular version will show the differences between that version and a previous version. Green highlighted text indicates new additions to the page, red highlighted text shows deletions to the page. From the main history page you are able to move back to a previous version via the "revert" link, this will however create a new version of the page itself.

## Tags (aka categories)
Roadkill differs slightly from other wiki engines in that it approaches categorizing pages from a blog engine perspective, by using tags rather than categories. Obviously the difference can be seen as just semantics, but tagging allows you use as many tags as you like for each page, giving a page multiple categories. These tags are also searchable in the Roadkill search interface, more on this on [Searching with the inbuilt Lucene.net search engine] page.

There is one special tag built into Roadkill which is the "homepage" tag. This can be used on one page only (the first page is always used), and tells Roadkill that this page should be used for the homepage on the site.

## Unsafe HTML
By default Roadkill will remove any HTML tags and attributes that aren't inside the ~/App_Data/Internal/htmlwhitelist.xml file. Any javascript, scripts, css behaviours are removed from the page output, and all attributes are also HTML encoded. More information on how the HTML sanitization works can be found in [this blog post](http://www.anotherchris.net/asp-net/anti-xss-net-libraries-a-hole-in-the-framework/).