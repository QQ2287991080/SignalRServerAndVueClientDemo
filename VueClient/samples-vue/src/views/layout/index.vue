<template>
  <div class="main">
    <div class="left">
      <left />
    </div>
    <div class="right">
      <right />
    </div>
  </div>
</template>

<script>
import left from "../layout/sidebar";
import right from "../layout/appMain";
export default {
  components: {
    left,
    right,
  },
};
</script>

<style>
.main {
  min-height: 100%;
  position: relative;
}
.left {
  width: 210px !important;
  height: 100%;
  position: fixed;
  font-size: 0px;
  top: 0;
  bottom: 0;
  left: 0;
  z-index: 1001;
  overflow: hidden;
  background-color: #304156;
}
.right {
  margin-left: 200px;
}
</style>