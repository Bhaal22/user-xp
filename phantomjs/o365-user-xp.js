var system = require('system');
var args = system.args;

var site = "";
var username = "";
var password = "";

var page = require('webpage').create();

var testindex = 0;
var loadInProgress = false;
page.settings.userAgent = 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/44.0.2403.157 Safari/537.36';
page.settings.javascriptEnabled = true;
page.settings.loadImages = true;//Script is much faster with this field set to false
phantom.cookiesEnabled = true;
phantom.javascriptEnabled = true;

page.onResourceRequested = function(request) {
  //console.log('Request       ' + JSON.stringify(request.url, undefined, 4));
  console.log('Request ' + request.url);
};

page.onResourceReceived = function(response) {
  
  //console.log('Receive ' + JSON.stringify(response, undefined, 4));
};

page.onLoadStarted = function() {
    loadInProgress = true;
    console.log('Loading started');
};
page.onLoadFinished = function() {
    loadInProgress = false;
    console.log('Loading finished');
};
page.onConsoleMessage = function(msg) {
    console.log(msg);
};


var sharepointSite = "https://mySite";
var o365AuthPage = "https://login.microsoftonline.com/";

page.onConsoleMessage = function(msg) {
  console.dir(msg);
}

var steps = [
  function() {
    page.open(site, function(status) {
      if(status === "success") {
      }
    });
  },

  function() {
    page.evaluate(function(username, password) {
      console.log(username);
      console.log(password);
      $("#cred_userid_inputtext").val(username);
      $("#cred_password_inputtext").val(password);
      $("#credentials").submit();
    }, username, password);
  },

  function() {
    var fs = require('fs');
    var result = page.evaluate(function() {
	    return document.querySelectorAll("html")[0].outerHTML;
    });
    fs.write('SharepointSite.html',result,'w');
  }
];

if (args.length !== 4) {
  console.log('Usage: phantomjs o365-user-xp.js $site $username $password');
  phantom.exit();
} else {
  site = args[1];
  username = args[2];
  password = args[3];
}


//Execute steps one by one
interval = setInterval(executeRequestsStepByStep,50);

function executeRequestsStepByStep(){
    if (loadInProgress == false && typeof steps[testindex] == "function") {
        steps[testindex]();
        testindex++;
    }
    if (typeof steps[testindex] != "function") {
        console.log("test complete!");
        phantom.exit();
    }
}
