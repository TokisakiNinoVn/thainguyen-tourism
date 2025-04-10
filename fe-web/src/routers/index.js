// src/routers/index.js
import { createRouter, createWebHistory } from 'vue-router';
import Home from '@/pages/HomePage.vue';
import Introduce from '@/pages/IntroducePage.vue';
import Login from '@/pages/LoginPage.vue';
import NotFound from '@/pages/NotFoundPage.vue';
import ComplaintsManagementPage from '@/pages/ComplaintsManagementPage.vue';
import CustomerManagerment from '@/pages/customers/CustomerManagerment.vue';
import InforPage from '@/pages/InforPage.vue';
import VoucherManagement from '@/pages/vouchers/VoucherManagement.vue';
import NotiManagement from '@/pages/noti/NotiManagement.vue';
import OrderManagement from '@/pages/order/OrderManagement.vue';
import OrderCreate from '@/pages/order/OrderCreate.vue';
import BlankPage from '@/pages/BlankPage.vue';

const routes = [
  { path: '/', component: Introduce },
  { path: '/blank', component: BlankPage },
  { path: '/login', component: Login },
  { path: '/complaints-management', component: ComplaintsManagementPage },
  { path: '/customer-list', component: CustomerManagerment },
  { path: '/user', component: InforPage },
  { path: '/vouchers', component: VoucherManagement },
  { path: '/notifications', component: NotiManagement },
  { path: '/orders', component: OrderManagement },
  { path: '/create-order', component: OrderCreate },

  { path: '/home', component: Home, meta: { requiresAuth: true } },
    
  { path: '/:pathMatch(.*)*', name: 'NotFound', component: NotFound }
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

// Navigation Guard
router.beforeEach((to, from, next) => {
  const isLogin = !!localStorage.getItem('isLoggedIn'); // Kiểm tra nếu có token trong localStorage

  if (isLogin == 'false') {
    next('/login'); // Nếu chưa đăng nhập, chuyển hướng đến trang đăng nhập
  } else {
    next(); // Cho phép truy cập trang
  }
});

export default router;
