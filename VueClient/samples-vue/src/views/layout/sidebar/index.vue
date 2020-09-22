<template>
  <el-menu
    :show-timeout="200"
    :collapse-transition="true"
    background-color="#304156"
    text-color="#f4f4f5"
    active-text-color="#409EFF"
    :unique-opened="false"
    router
    mode="vertical"
  >
    <sidebar-item :routes="routes"></sidebar-item>
  </el-menu>
</template>

<script>
import sidebarItem from "./mysidebar";
export default {
  components: { sidebarItem },
  data() {
    return {};
  },
  computed: {
    routes() {
      //返回菜单
      return this.$router.options.routes;
    },
  },
};
</script>
</style>
