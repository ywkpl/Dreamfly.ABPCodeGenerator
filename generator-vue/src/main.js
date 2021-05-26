import { createApp } from "vue";
import App from "./App.vue";
import router from "./router";
import store from "./store";
import { Button } from "ant-design-vue";

createApp(App).use(Button).use(store).use(router).mount("#app");
