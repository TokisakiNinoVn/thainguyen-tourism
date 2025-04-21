import { createRouter, createWebHistory } from "vue-router";
import Dashboard from "../views/Dashboard.vue";

import ManagementPlace from "@/views/place/ManagementPlace.vue";
import AddPlace from "@/views/place/AddPlace.vue";
import UpdatePlace from "@/views/place/UpdatePlace.vue";
import ManagementMediaPlace from "@/views/place/UpdatePlaceMedia.vue";

import Billing from "../views/Billing.vue";
import VirtualReality from "../views/VirtualReality.vue";
// import RTL from "../views/Rtl.vue";
import Profile from "../views/Profile.vue";
import Signup from "../views/Signup.vue";
import Signin from "../views/Signin.vue"; // login page

// Khai báo routes
const routes = [
  { path: "/", name: "/", redirect: "/dashboard-default" },
  { path: "/dashboard-default", name: "Dashboard", component: Dashboard, meta: { requiresAuth: true } },

  { path: "/management-place", name: "ManagementPlace", component: ManagementPlace, meta: { requiresAuth: true } },
  { path: "/add-place", name: "Thêm địa điểm", component: AddPlace, meta: { requiresAuth: true } },
  { path: "/places/edit/:id", name: "Sửa thông tin địa điểm", component: UpdatePlace, meta: { requiresAuth: true } },
  { path: "/places/media/:id", name: "Files", component: ManagementMediaPlace, meta: { requiresAuth: true } },


  { path: "/billing", name: "Billing", component: Billing, meta: { requiresAuth: true } },
  { path: "/virtual-reality", name: "Virtual Reality", component: VirtualReality, meta: { requiresAuth: true } },
  // { path: "/rtl-page", name: "RTL", component: RTL, meta: { requiresAuth: true } },
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
