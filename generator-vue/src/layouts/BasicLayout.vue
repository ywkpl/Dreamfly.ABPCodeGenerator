<template lang="html">
  <Header />
  <SideMenu />
  <router-view />
  <Footer />
</template>
<script>
import Header from "./Header";
import Footer from "./Footer";
import SideMenu from "./SideMenu";
export default {
  components: { Header, Footer, SideMenu },
};
</script>
<style lang="less"></style>
