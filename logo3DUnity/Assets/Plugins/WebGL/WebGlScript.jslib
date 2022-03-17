mergeInto(LibraryManager.library, {

  // GetIsPC: function () {
  //   var userAgentInfo = navigator.userAgent;
  //   var Agents = ["Android", "iPhone", "SymbianOS", "Windows Phone", "iPad", "iPod"];
  //   var flag = true;
  //   for (var v = 0; v < Agents.length; v++)
  //   {
  //     if (userAgentInfo.indexOf(Agents[v]) > 0)
  //     {
  //         flag = false;
  //         break;
  //     }
  //   }
  //   return flag;
  // },

  // PlayVideo : function (path){
  //   showVideo(Pointer_stringify(path));
  // },

  // fullScreen : function(){
  //   var docElm = document.documentElement
  //   if(docElm.mozRequestFullScreen) {
  //     docElm.mozRequestFullScreen();
  //   }
  //   console.log("11111")
  // },

  // ShowStartToFullScreen : function(){
  //   var startPanel = document.getElementById("startPanel");
  //   startPanel.removeAttribute("hidden");
  // },

  UnityMessage: function (msg) {
    onUnityMessage(Pointer_stringify(msg));
  }

});