import { createRouter, createWebHistory } from "vue-router";
import NProgress from "nprogress";
import "nprogress/nprogress.css";
// import { h, resolveComponent } from "vue";

const routes = [
  {
    path: "/",
    redirect: "/setting",
  },
  {
    path: "/setting",
    //添加布局组件，必须包含router-view子组件，下阶路由时用到
    component: () =>
      import(
        /* webpackChunkName: "siderLayout" */ "../layouts/SiderLayout.vue"
      ),
    // component: {
    //   render: () => h(resolveComponent("router-view")),
    // },
    //添加下阶路由
    children: [
      {
        path: "/setting",
        name: "setting",
        meta: { icon: "HomeOutlined", title: "首页" },
        component: () =>
          import(/* webpackChunkName: "setting" */ "../views/Home.vue"),
      },
      {
        path: "/setting/project",
        name: "project",
        meta: { icon: "ProfileOutlined", title: "项目设定" },
        component: () =>
          import(/* webpackChunkName: "setting" */ "../views/Setting/Project"),
      },
      {
        path: "/setting/entity",
        name: "entity",
        meta: { icon: "CoffeeOutlined", title: "实体设定" },
        component: () =>
          import(/* webpackChunkName: "setting" */ "../views/Setting/Entity"),
      },
    ],
  },
  {
    hideInMenu: true,
    path: "/:catchAll(.*)",
    name: "notFound",
    component: () => import(/* webpackChunkName: "about" */ "../views/404.vue"),
  },
];

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes,
});

router.beforeEach((to, from, next) => {
  NProgress.start();
  next();
});

router.afterEach(() => {
  NProgress.done();
});
export default router;
