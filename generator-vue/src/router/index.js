import { createRouter, createWebHistory } from "vue-router";
import Home from "../views/Home.vue";
//import { h, resolveComponent } from "vue";

const routes = [
  {
    path: "/setting",
    //添加布局组件，必须包含router-view子组件，下阶路由时用到
    component: () =>
      import(
        /* webpackChunkName: "basicLayout" */ "../layouts/BasicLayout.vue"
      ),
    // component: {
    //   render: () => h(resolveComponent("router-view")),
    // },
    //添加下阶路由
    children: [
      {
        path: "/setting",
        redirect: "/setting/project",
      },
      {
        path: "/setting/project",
        name: "project",
        component: () =>
          import(/* webpackChunkName: "setting" */ "../views/Setting/Project"),
      },
      {
        path: "/setting/entity",
        name: "entity",
        component: () =>
          import(/* webpackChunkName: "setting" */ "../views/Setting/Entity"),
      },
    ],
  },
  {
    path: "/",
    name: "Home",
    component: Home,
  },
  {
    path: "/:catchAll(.*)",
    name: "notFound",
    component: () => import(/* webpackChunkName: "about" */ "../views/404.vue"),
  },
];

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes,
});

export default router;
