<template>
    <canvas id="canvas"></canvas>
</template>

<script>
export default {
    data(){
        return {
            width: 0,
            height: 0,
            canvas: null,
            ctx: null,
            points: [],
            // timer
            timerID: 0,
            lastFrameTime: new Date().getTime(),
            deltaTime: 0,
            //createPoints
            nextTime: new Date(),


        }
    },
    mounted() {
        this.initCanvas()
        this.startTimer()
        // 当调整窗口大小时重绘canvas
        window.onresize = () => {
            this.initCanvas()
        }
    },
    methods: {
        // createPoints
        startTimer() {
            window.cancelAnimationFrame(this.timerID)
            this.requestFrame()
        },
        requestFrame(){
            this.timerID = window.requestAnimationFrame(()=>{
                this.update()
                this.requestFrame() // 下一帧
            })
        },
        update(){
            // createPoints
            let now = new Date()
            this.deltaTime = (now.getTime() - this.lastFrameTime) / 1000
            this.lastFrameTime = now.getTime()
            if (now.getTime() >= this.nextTime) {
                this.createPoints()
                const r = Math.random()
                this.nextTime = now.setMilliseconds(now.getMilliseconds() + r * 150 + 50);
            }
            //movePoints
            this.movePoints()

            this.draw()
        },
        createPoints(){
            // console.log("create");
            for (let i = 0; i < 2; i ++) {
                this.points.push({
                    x: this.width * Math.random(),
                    y: -10,
                    a: 500,
                    speed: 1000 * Math.random() + 200,
                })
            }
        },
        movePoints(){
            for (let i = this.points.length - 1; i >=0; i --) {
                let p = this.points[i];
                p.speed += p.a * this.deltaTime
                p.y += p.speed * this.deltaTime
                if (p.y >= this.height) {
                    // console.log("delete")
                    this.points.splice(i, 1);
                }
            }
        },
        initCanvas() {
            this.width = window.innerWidth
            this.height = window.innerHeight
            // console.log("初始化canvas")
            this.canvas = document.getElementById('canvas');
            this.ctx = this.canvas.getContext('2d');
            this.canvas.width = this.width
            this.canvas.height = this.height
            if (this.timerID <= 0) {
                this.draw()
            }
        },
        draw(){
            if (!this.ctx) {
                return
            }
            // draw background
            this.ctx.fillStyle="#FFFFFF44";
            // this.ctx.fillStyle="#ffffff";
            this.ctx.fillRect(0, 0, this.width, this.height);
            //test
            // this.drawSmile()
            //draw points
            // this.ctx.beginPath();
            // for (const p of this.points) {
            //     this.ctx.moveTo(p.x + 5, p.y);
            //     this.ctx.arc(p.x, p.y, 5, 0, Math.PI * 2, true);
            // }
            // this.ctx.stroke();
            for (const p of this.points) {
                // this.ctx.fillStyle = "#333377";
                this.ctx.beginPath();
                this.ctx.moveTo(p.x, p.y);
                let endY = p.y - p.speed * 0.1;
                this.ctx.lineTo(p.x, endY);
                var gnt1 = this.ctx.createLinearGradient(p.x, p.y, p.x, endY);//线性渐变的起止坐标
                gnt1.addColorStop(0,'#aaaaff88');//创建渐变的开始颜色，0表示偏移量，个人理解为直线上的相对位置，最大为1，一个渐变中可以写任意个渐变颜色
                gnt1.addColorStop(1,'#aaffff00');
                this.ctx.lineWidth=2;
                this.ctx.strokeStyle = gnt1;
                this.ctx.stroke();
            }
        },
        drawSmile() {
            this.ctx.beginPath();
            this.ctx.arc(75, 75, 50, 0, Math.PI * 2, true); // 绘制
            this.ctx.moveTo(110, 75);
            this.ctx.arc(75, 75, 35, 0, Math.PI, false); // 口(顺时针)
            this.ctx.moveTo(65, 65);
            this.ctx.arc(60, 65, 5, 0, Math.PI * 2, true); // 左眼
            this.ctx.moveTo(95, 65);
            this.ctx.arc(90, 65, 5, 0, Math.PI * 2, true); // 右眼
            this.ctx.stroke();
        }
    }
}
</script>