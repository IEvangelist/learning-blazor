const renderTweets = () => {
    if (this.twttr && this.twttr.widgets) {
        this.twttr.widgets.load();
    }
}

// Register the twitter object, with a single function available in it.
window.twitter = {
    renderTweets
};

// Register the twitter-provided widget to render the DOM updates.
// This is called from the .NET TweetComponent.razor.cs file.
window.twttr = (function (d, s, id) {
    var js,
        fjs = d.getElementsByTagName(s)[0],
        t = window.twttr || {};
    if (d.getElementById(id)) return t;
    js = d.createElement(s);
    js.id = id;
    js.src = "https://platform.twitter.com/widgets.js";
    fjs.parentNode.insertBefore(js, fjs);

    t._e = [];
    t.ready = function (f) {
        t._e.push(f);
    };

    return t;
}(document, "script", "twitter-wjs"));
