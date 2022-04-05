<template>
    <div ref="root" class="logo-root">
    </div>
</template>

<script>
import * as THREE from 'three';
import { GLTFLoader } from 'three/examples/jsm/loaders/GLTFLoader.js';
export default {
    data() {
        return {
            scene: null,
            camera: null,
            renderer: null,
            mesh: null
        }
    },
    mounted() {
        this.initScene()
    },
    methods: {
        initScene(){
            this.camera = new THREE.PerspectiveCamera( 30, 300 / 300, 0.01, 10 );
            this.camera.position = {x: 0, y: 0.5, z: 2}
            this.camera.rotation.x = -0.2


            this.scene = new THREE.Scene();
            this.scene.add(new THREE.AmbientLight(0x666666)); //环境光

            const light = new THREE.DirectionalLight(0xdddddd, 1); //从正上方（不是位置）照射过来的平行光，0.45的强度
            light.position.set(-5, 0, 10);
            // light.position.multiplyScalar(1);
            this.scene.add(light);

            this.renderer = new THREE.WebGLRenderer( { antialias: true, alpha: true} );
            this.renderer.setSize(300, 300);

            this.$el.appendChild( this.renderer.domElement );
            
            const loader = new GLTFLoader();
            loader.load( 'three/vueLogo.glb', ( gltf ) => {
                this.mesh = gltf.scene.children[0]
                console.log(this.mesh)
                this.mesh.scale.set(0.2, 0.2, 0.2);
                this.scene.add( this.mesh );

                // this.renderer.render(this.scene, this.camera)
                this.animate()
            }, undefined, function ( error ) {
                console.error( error );
            });

            
        },
        animate: function () {
            requestAnimationFrame(this.animate)
            // this.mesh.rotation.x += 0.01
            this.mesh.rotation.y += 0.02
            this.renderer.render(this.scene, this.camera)
        }
    }
}
</script>

<style lang="scss" scoped>
.logo-root{
    width: 100%;
    height: 100%;
    // background-color: gray;
}
</style>