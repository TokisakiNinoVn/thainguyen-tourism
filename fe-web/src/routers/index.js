// src/routers/index.js
import { createRouter, createWebHistory } from 'vue-router';
// import HomePage from '@/pages/HomePage.vue';
import HomePage from '@/pages/HomePage.vue';
import Login from '@/pages/LoginPage.vue';
import NotFound from '@/pages/NotFoundPage.vue';
// import ComplaintsManagementPage from '@/pages/ComplaintsManagementPage.vue';
// import CustomerManagerment from '@/pages/customers/CustomerManagerment.vue';
import IntroducePage from '@/pages/IntroducePage.vue';
// import VoucherManagement from '@/pages/vouchers/VoucherManagement.vue';
// import NotiManagement from '@/pages/noti/NotiManagement.vue';
// import OrderManagement from '@/pages/order/OrderManagement.vue';
// import OrderCreate from '@/pages/order/OrderCreate.vue';
import BlankPage from '@/pages/BlankPage.vue';
import AccountInfor from '@/pages/account/AccountInfor.vue';

import ListPlace from '@/pages/place/ListPlace.vue';
import DetailsPlace from '@/pages/place/DetailsPlace.vue';
import DetailsPlaceMedia from '@/pages/place/DetailsPlaceMedia.vue';
import ChatPage from '@/pages/chat/ChatPage.vue';
import CreateBlog from '@/pages/blog/CreateBlog.vue';
import ListBlog from '@/pages/blog/ListBlog.vue';
import DetailsBlog from '@/pages/blog/DetailsBlog.vue';

const routes = [
  { path: '/', component: HomePage },
  { path: '/blank', component: BlankPage },
  { path: '/create-blog', component: CreateBlog, meta: { requiresAuth: true } },
  { path: '/account', component: AccountInfor, meta: { requiresAuth: true } },

  { path: '/login', component: Login },
  { path: '/places', component: ListPlace },
  { path: '/places/:id', component: DetailsPlace },
  { path: '/places/media/:id', component: DetailsPlaceMedia },

  { path: '/blogs', component: ListBlog },
  { path: '/blog/detail/:id', component: DetailsBlog },
  { path: '/introduce', component: IntroducePage },

  // { path: '/home', component: HomePage, meta: { requiresAuth: true } },
  { path: '/chat', component: ChatPage, meta: { requiresAuth: true } },
    
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
