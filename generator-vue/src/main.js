import { createApp } from "vue";
import App from "./App.vue";
import router from "./router";
import store from "./store";
import { Button, Layout } from "ant-design-vue";

createApp(App).use(Button).use(Layout).use(store).use(router).mount("#app");
