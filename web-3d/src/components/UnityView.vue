<template>
    <div id="unityContainer" class="unityView" :class="{unityViewAnim: showingAnim, unityViewShown: show}"></div>
</template>

<script>
export default {
    props:{
        jsonPath: String, // json文件
        hideLaunch:{ // 隐藏启动动画（启动动画算入加载时间中）
            type: Boolean,
            default: true,
        },
        showingAnim:{ // 加载完渐变显示，或直接显示
            type: Boolean,
            default: true,
        }
    },
    data(){
        return {
            startLoadingTime: 0, //开始加载时间
            estimateEndTime: 0, //预计结束时间
            show : false, //启动动画结束回调（还需要+0.4s的减淡动画时间）
            loaderTimerId: 0, //启动update计时
        }
    },
    created() {
        this.startLoadingTime = new Date().getTime(); //记录启动开始时间
        window.UnityLoader.instantiate("unityContainer", this.jsonPath, {onProgress: this.onUnityProgress});
        window.addEventListener("unityMessageEvent", this.onUnityMessage); //自定义event的接收
        //loading刷新
        this.loaderTimerId = setInterval(() => {
            this.timerUpdate();
        }, 33); //30fps
    },
    beforeDestroy() {
        window.removeEventListener("unityMessageEvent", this.onUnityMessage);
        clearInterval(this.loaderTimerId);
    },
    methods:{
        //加载进度回调
        onUnityProgress(gameInstance, progress) {
            // console.log(progress);
            if (progress == 0) {
                this.estimateEndTime = 0;
                return;
            }
            const now = new Date().getTime()
            let luanchTime = 2300; //加载完成(progress==1)后，还有启动动画，测试2.3s
            if (!this.hideLaunch) {
                luanchTime = 0; //如果不隐藏启动图，则加载时间不算启动图
            }
            if (progress < 1) {
                const pastTime = now - this.startLoadingTime;
                this.estimateEndTime = this.startLoadingTime + pastTime / progress + luanchTime; //加载完还有3s启动动画
            }else{
                this.estimateEndTime = now + luanchTime;
            }
            // console.log("estimateEndTime:" + this.estimateEndTime);
            },
            //刷新（在加载进度==1后不再有回调，但是还算上启动动画时间，也要有刷新）
            timerUpdate(){
            if (this.estimateEndTime == 0) {
                return;
            }
            const now = new Date().getTime()
            const pastTime = now - this.startLoadingTime;
            let progress = pastTime / (this.estimateEndTime - this.startLoadingTime);
            if (progress >= 1) {
                progress = 1;
                clearInterval(this.loaderTimerId);
                if (!this.hideLaunch) {
                    this.show = true
                    this.$emit("onLoadFinish");
                }
            }
            this.$emit("onProgress", progress);
        },
        //unity通知事件
        onUnityMessage(e) {
            // console.log(e);
            if (e.msg == "loaded") {
                // 隐藏启动图时，才监听loaded事件，否则已经直接显示unity画面
                if (this.hideLaunch) {
                    //收到unity已启动通知
                    //当前还有从启动图渐变到正式场景的过渡动画。延迟0.4s
                    setTimeout(() => {
                        this.show = true
                        this.$emit("onLoadFinish");
                    }, 400);
                }
            }
        }
    }
}
</script>

<style scoped>
.unityView{
    width: 100%;
    height: 100%;
    opacity: 0;
}
.unityViewAnim{
    transition: opacity 0.5s;
}
.unityViewShown{
    opacity: 1;
}
</style>