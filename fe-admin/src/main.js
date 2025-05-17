import { createApp } from "vue";
import App from "./App.vue";
import store from "./store";
import router from "./router";
import "./assets/css/nucleo-icons.css";
import "./assets/css/nucleo-svg.css";
import ArgonDashboard from "./argon-dashboard";
import VueApexCharts from "vue3-apexcharts";

if (!localStorage.getItem('isLoggedIn')) {
    localStorage.setItem('isLoggedIn', 'false');
}
const appInstance = createApp(App);
appInstance.component("apexchart", VueApexCharts);
appInstance.use(store);
appInstance.use(router);
appInstance.use(ArgonDashboard);
appInstance.mount("#app");


