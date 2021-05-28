<template lang="html">
  <a-layout style="min-height: 100vh">
    <a-layout-sider :trigger="null" v-model:collapsed="collapsed" collapsible>
      <div class="logo" />
      <!-- <a-button @click="changeCollapsed">test</a-button> -->
      <SideMenu />
    </a-layout-sider>
    <a-layout>
      <a-layout-header style="background: #fff; padding: 0">
        <div>
          <MenuFoldOutlined
            v-show="collapsed"
            class="trigger"
            @click="collapse"
          />
          <MenuUnfoldOutlined
            v-show="!collapsed"
            class="trigger"
            @click="collapse"
          />
        </div>
        <Header />
      </a-layout-header>
      <a-layout-content style="margin: 0 16px">
        <router-view />
      </a-layout-content>
      <a-layout-footer style="text-align: center">
        <Footer />
      </a-layout-footer>
    </a-layout>
  </a-layout>
</template>
<script>
import Header from "./Header";
import Footer from "./Footer";
import SideMenu from "./SideMenu";
import { MenuFoldOutlined, MenuUnfoldOutlined } from "@ant-design/icons-vue";
import { ref } from "vue";
export default {
  setup() {
    let collapsed = ref(false);
    let collapse = () => (collapsed.value = !collapsed.value);
    return {
      collapsed,
      collapse,
    };
  },
  // data() {
  //   return {
  //     collapsed: false,
  //   };
  // },
  components: {
    Header,
    Footer,
    SideMenu,
    MenuFoldOutlined,
    MenuUnfoldOutlined,
  },
};
</script>
<style lang="less">
.trigger {
  padding: 0 20px;
  line-height: 64px;
  font-size: 20px;
}
.trigger:hover {
  background: #eeeeee;
}
</style>
