<template>
  <div style="width: 256px">
    <a-menu
      theme="dark"
      v-model:openKeys="openKeys"
      v-model:selectedKeys="selectedKeys"
    >
      <!--TODO 跟动态HX 标记一个道理，后缀可以优化-->
      <a-menu-item
        v-for="item in list"
        :key="item.path"
        @click="() => router.push({ path: item.path })"
      >
        <CoffeeOutlined v-if="item.meta.icon == 'CoffeeOutlined'" />
        <ProfileOutlined v-if="item.meta.icon == 'ProfileOutlined'" />
        <HomeOutlined v-if="item.meta.icon == 'HomeOutlined'" />
        <span>{{ item.meta.title }}</span>
      </a-menu-item>
    </a-menu>
  </div>
</template>
<script lang="javascript">
import { reactive, toRefs, watch } from "vue";
import {
  CoffeeOutlined,
  ProfileOutlined,
  HomeOutlined,
} from "@ant-design/icons-vue";
import { useRouter } from "vue-router";
export default {
  setup() {
    const state = reactive({
      selectedKeys: ["1"],
      openKeys: ["sub1"],
      preOpenKeys: ["sub1"],
      list: [],
    });

    const router = useRouter();

    watch(
      () => state.openKeys,
      (val, oldVal) => {
        state.preOpenKeys = oldVal;
      }
    );
    const getMenuData = () => {
      return router.options.routes[1].children;
    };

    state.list = getMenuData();
    return {
      ...toRefs(state),
      router,
    };
  },
  components: {
    CoffeeOutlined,
    ProfileOutlined,
    HomeOutlined,
  },
};
</script>
<style lang="less" scoped></style>
