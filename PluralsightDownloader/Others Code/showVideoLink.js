var showList = function(){
	var contentList = [];
	contentList.push($(".left h1").text());
	var playList = $(".linkContainer.ng-scope tr.module span"); 
	for(var i=0;i<playList.length;i++){
		contentList.push(playList[i].innerHTML);
	}
	console.log(contentList.join(';'));
};
var storePlayList = (function() {
    showList();
	var nodeList = $(".linkContainer.ng-scope tr.module");
	var playList = [];
	for(var i=0; i<nodeList.length; i++){
		var info = $(".linkContainer.ng-scope tr.module:eq(0) a").attr("ng-click");
		info = info.substring(info.indexOf("author="),info.lastIndexOf("'"));
	    var reg = /author=([\w|-]+).+course=([\w|-]+).+/;
	    var author = reg.exec(info)[1];
	    var course = reg.exec(info)[2];
		var playInfo = {};
		playInfo.length = $(".linkContainer.ng-scope tr.module:eq("+i+")~.tocClips").length-$(".linkContainer.ng-scope tr.module:eq("+(i+1)+")~.tocClips").length;
		playInfo.json =  {a:author, m:nodeList.eq(i).attr("id"), course:course, cn:0, mt:"mp4", q:"1024x768", cap:false, lc:"en"};       
		playList.push(playInfo);
	}
	return playList;
	//localStorage.playList = JSON.stringify(playList);
}());

var checkListDone = function(playList){

};

var setSingleUrl = function(playList,i){
	var rjson = playList.json;
	rjson.cn = i;
	var xhr = $.post("/training/Player/ViewClip", rjson);
	xhr.always(function(e){
		if(e.status==200){
			$(".left h1 a")[i].href=e.responseText;
			videoList[start].push(start+"-"+(i+1)+","+e.responseText.substring(7)+";\r\n");
			playList[i].processInfo = "done";
			//console.log(start+"-"+(i+1)+","+e.responseText.substring(7)+";");
	 		//console.log(i+" clicked!");
			//$(".left h1 a")[i].click();
		}
		else if(e.status==407){
			setSingleUrl(playList,i);
		}
		else{
			console.log(i+" failed!   "+e.status+","+e.responseText);
			playList[i].processInfo = "failed :"+e.status+","+e.responseText;			
		}
  	});
};
var getVideos = function(playList){
	$(".left h1 a").remove();
	for (var i =0; i<playList.length; i++){
		(function(){
			var a = i;
			setTimeout(function(){
        	$(".left h1").append("<a href='' download='"+a+".mp4'>"+a+"</a>");
        	videoList[start] = [];
			setSingleUrl(playList,a);
         	}, 5000*i);
		})();
	}
};

var start = 0;
var videoList = {};
var r=function(i){
	setSingleUrl(storePlayList[start-1],i);
};
var q=function(){
  if(start<storePlayList.length){
    getVideos(storePlayList[start]);
    start++;
  }
  else{
    console.log("All Done!");
  }  
};
