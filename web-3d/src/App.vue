<template>
  <div id="app">
    <canvas-bg class="bg"></canvas-bg>
    <HelloWorld msg="Welcome to Your Vue.js App"/>
  </div>
</template>

<script>
import CanvasBg from './components/CanvasBg.vue'
import HelloWorld from './components/HelloWorld.vue'

export default {
  name: 'App',
  components: {
    CanvasBg,
    HelloWorld
  }
}
</script>

<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  /* margin-top: 20px; */
}
.bg{
  z-index: -1;
  position: absolute;
  left: 0;
  top: 0;
  margin: 0;
  width: 100%;
  height: 100vh;
}
</style>
