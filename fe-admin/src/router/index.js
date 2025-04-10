import { createRouter, createWebHistory } from "vue-router";
import Dashboard from "../views/Dashboard.vue";
import Tables from "../views/Tables.vue";
import Billing from "../views/Billing.vue";
import VirtualReality from "../views/VirtualReality.vue";
import RTL from "../views/Rtl.vue";
import Profile from "../views/Profile.vue";
import Signup from "../views/Signup.vue";
import Signin from "../views/Signin.vue"; // login page

// Khai báo routes
const routes = [
  { path: "/", name: "/", redirect: "/dashboard-default" },
  { path: "/dashboard-default", name: "Dashboard", component: Dashboard, meta: { requiresAuth: true } },
  { path: "/tables", name: "Tables", component: Tables, meta: { requiresAuth: true } },
  { path: "/billing", name: "Billing", component: Billing, meta: { requiresAuth: true } },
  { path: "/virtual-reality", name: "Virtual Reality", component: VirtualReality, meta: { requiresAuth: true } },
  { path: "/rtl-page", name: "RTL", component: RTL, meta: { requiresAuth: true } },
  { path: "/profile", name: "Profile", component: Profile, meta: { requiresAuth: true } },
  { path: "/login", name: "Signin", component: Signin },
  { path: "/signup", name: "Signup", component: Signup },
];

// Khởi tạo router
const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes,
  linkActiveClass: "active",
});

// Middleware kiểm tra token trong localStorage
router.beforeEach((to, from, next) => {
  const isProtected = to.meta.requiresAuth;
  const token = localStorage.getItem("token");

  if (isProtected && !token) {
    return next("/login");
  }

  next();
});

export default router;
