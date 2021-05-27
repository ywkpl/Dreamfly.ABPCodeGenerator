import { createRouter, createWebHistory } from "vue-router";
import Home from "../views/Home.vue";
//import RenderRouterView from "../components/RenderRouterView.vue";
import { h } from "vue";

const routes = [
  {
    path: "/setting",
    // component: {
    //   render: () => {
    //     return h("router-view", {});
    //   },
    // },
    component: {
      render: () => {
        return h("h1", {}, "AA");
      },
    },
    children: [
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
    path: "/about",
    name: "About",
    // route level code-splitting
    // this generates a separate chunk (about.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    component: () =>
      import(/* webpackChunkName: "about" */ "../views/About.vue"),
  },
];

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes,
});

export default router;
