var courseInfoList,playList,autoGet;
var start = 0;
var videoList = [];
var contentList = [];
var storePlayList = function() {
	playList = [];
	for(var i in courseInfoList){
		var playListInfo = [];
		for(var j in courseInfoList[i].clips){
			var courseInfo = courseInfoList[i].clips[j];
			var reg = /author=([\w|-]+).+name=([\w|-]+).+course=([\w|-]+)/;
		    var author = reg.exec(courseInfo.playerParameters)[1];
		    var name = reg.exec(courseInfo.playerParameters)[2];
		    var course = reg.exec(courseInfo.playerParameters)[3];
		    //var playInfo =  {a:author, m:name, course:course, cn:j, mt:"mp4", q:"1024x768", cap:false, lc:"en"};   
		    var playInfo =  {a:author, m:name, course:course, cn:j, mt:"mp4", q:"1280x720", cap:false, lc:"en"};   
			playListInfo.push(playInfo);
		}
		playList.push(playListInfo);
	}
};
var showList = function(){
	var courseLink = "http://www.pluralsight.com/data/Course/Content/"+document.location.pathname.substr(9);
	contentList.push($(".row h1").text());
	$.get(courseLink,function(courses){
		courseInfoList = courses;
		for(var i in courseInfoList){
			contentList.push(courseInfoList[i].title);
		}
		console.log(contentList.join(';'));
		storePlayList();
	});
}();
var checkListDone = function(start,i){
	if(i==playList[start].length-1){
		console.log(videoList[start].join(""));
		if(autoGet){
			setTimeout(function(){
				q();
	        }, 90000);
        }
	}
};
var getSingleVideo = function(start,i){
	var rjson = playList[start][i];
	var xhr = $.post("/training/Player/ViewClip", rjson);
	xhr.always(function(e){
		if(e.status==200){
			var url = e.responseText.substring(7);
			var downloadLink = (start+1)+"-"+url.split('/')[6]+","+url+";\r\n";
			videoList[start].push(downloadLink);
			//console.log(downloadLink);
			playList[start][i].processInfo = "done";
		}
		else if(e.status==407){
			getSingleVideo(start,i);
		}
		else{
			console.log(i+" failed!   "+e.status+","+e.responseText);
			playList[start][i].processInfo = "failed :"+e.status+","+e.responseText;			
		}
		checkListDone(start,i);
  	});
};
var getVideos = function(start){
	videoList[start] = [];
	for (var i =0; i<playList[start].length; i++){
		(function(){
			var a = i;
			setTimeout(function(){
			getSingleVideo(start,a);
         	}, 6300*i);
		})();
	}
};

var q=function(){
  if(start<playList.length){
    getVideos(start);
    start++;
  }
  else{
    console.log("All Done!");
  }  
};
