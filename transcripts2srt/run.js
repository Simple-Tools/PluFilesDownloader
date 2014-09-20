(function() {
	var fs = require("fs");
	var CourseSrts = [];
	var SecondToTime = function(t) {
		var h = Math.floor(t / (60 * 60));
		h = h > 9 ? h : "0" + h;
		var m = Math.floor((t - 60 * 60 * h) / 60);
		m = m > 9 ? m : "0" + m;
		var s = Math.floor(t - 60 * 60 * h - 60 * m);
		s = s > 9 ? s : "0" + s;
		return h + ":" + m + ":" + s;
	};
	var ClipToSrt = function(i, j, clip) {
		j = (j+1) >9 ? (j+1) : "0" + (j+1);
		var srtName = (i+1) + "-" + clip.playerParameters.split("&")[1].split("=")[1] + "-" + j + ".srt";
		var srtContent = "";
		for (var i = 0; i < clip.transcripts.length; i++) {
			var toTime = i == (clip.transcripts.length - 1) ? (clip.transcripts[i].displayTime + 20) : clip.transcripts[i + 1].displayTime;
			srtContent += (i + 1) + "\n" + SecondToTime(clip.transcripts[i].displayTime) + ",0 --> " + SecondToTime(toTime) + ",0\n" + clip.transcripts[i].text + "\n";
		}
		//debugger
		return {
			"srtName": srtName,
			"srtContent": srtContent
		};
	};
	var GetCourseSrts = function(course) {
		//debugger
		for (var i = 0; i < course.modules.length; i++) {
			for (var j = 0; j < course.modules[i].clips.length; j++) {
				CourseSrts.push(ClipToSrt(i, j, course.modules[i].clips[j]));
			};
		};
	};
	var readFile = function(filepath) {
		fs.readFile(filepath, function(err, data) {
			if (err) throw err;
			jsonData = JSON.parse(data);
			console.log(jsonData);
			//debugger
			GetCourseSrts(jsonData);
			splitFiles();
		});
	};
	var writeFile = function(filepath, content) {
		fs.writeFile(filepath, content, function(err) {
			if (err) throw err;
			console.log(filepath + " created!\r\n");
		});
	};
	var splitFiles = function() {
		//debugger
		for (var i in CourseSrts) {
			writeFile("transcripts/" + CourseSrts[i].srtName, CourseSrts[i].srtContent);
		}
	};
	readFile("using-typescript-large-angularjs-apps.json");
})();