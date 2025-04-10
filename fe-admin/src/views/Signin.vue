<template>
  <main class="mt-0 main-content">
    <section>
      <div class="page-header min-vh-100">
        <div class="container">
          <div class="row">
            <div class="mx-auto col-xl-4 col-lg-5 col-md-7 d-flex flex-column mx-lg-0">
              <div class="card card-plain">
                <div class="pb-0 card-header text-start">
                  <h4 class="font-weight-bolder">Đăng nhập</h4>
                  <p class="mb-0">Nhập email và mật khẩu để đăng nhập</p>
                </div>
                <div class="card-body">
                  <form @submit.prevent="handleLogin">
                    <div class="mb-3">
                      <argon-input
                        v-model="email"
                        id="email"
                        type="email"
                        placeholder="Email"
                        name="email"
                        size="lg"
                      />
                    </div>
                    <div class="mb-3">
                      <argon-input
                        v-model="password"
                        id="password"
                        type="password"
                        placeholder="Password"
                        name="password"
                        size="lg"
                      />
                    </div>
                    <argon-switch v-model="rememberMe" id="rememberMe" name="remember-me">
                      Remember me
                    </argon-switch>

                    <div class="text-center">
                      <argon-button
                        type="submit"
                        class="mt-4"
                        variant="gradient"
                        color="success"
                        fullWidth
                        size="lg"
                      >
                        Sign in
                      </argon-button>
                    </div>
                  </form>
                </div>
                <div class="px-1 pt-0 text-center card-footer px-lg-2">
                  <p class="mx-auto mb-4 text-sm">
                    Don't have an account?
                    <a href="javascript:;" class="text-success text-gradient font-weight-bold">Sign up</a>
                  </p>
                </div>
              </div>
            </div>

            <div class="top-0 my-auto text-center col-6 d-lg-flex d-none h-100 pe-0 position-absolute end-0 justify-content-center flex-column">
              <div
                class="position-relative bg-gradient-primary h-100 m-3 px-7 border-radius-lg d-flex flex-column justify-content-center overflow-hidden"
                style="background-image: url('https://raw.githubusercontent.com/creativetimofficial/public-assets/master/argon-dashboard-pro/assets/img/signin-ill.jpg'); background-size: cover;"
              >
                <span class="mask bg-gradient-success opacity-6"></span>
                <h4 class="mt-5 text-white font-weight-bolder position-relative">
                  "Attention is the new currency"
                </h4>
                <p class="text-white position-relative">
                  The more effortless the writing looks, the more effort the writer actually put into the process.
                </p>
              </div>
            </div>

          </div>
        </div>
      </div>
    </section>
  </main>
</template>

<script setup>
import { onBeforeUnmount, onBeforeMount, ref } from "vue";
import { useStore } from "vuex";
import ArgonInput from "@/components/ArgonInput.vue";
import ArgonSwitch from "@/components/ArgonSwitch.vue";
import ArgonButton from "@/components/ArgonButton.vue";
import { login } from '@/apis/modules/auth.api';

const store = useStore();
const body = document.getElementsByTagName("body")[0];

onBeforeMount(() => {
  store.state.hideConfigButton = true;
  store.state.showNavbar = false;
  store.state.showSidenav = false;
  store.state.showFooter = false;
  body.classList.remove("bg-gray-100");
});

onBeforeUnmount(() => {
  store.state.hideConfigButton = false;
  store.state.showNavbar = true;
  store.state.showSidenav = true;
  store.state.showFooter = true;
  body.classList.add("bg-gray-100");
});

// Login form logic
const email = ref('');
const password = ref('');
const rememberMe = ref(false);

const handleLogin = async () => {
  try {
    const response = await login({ email: email.value, password: password.value });
    console.log("Login success:", response);
    localStorage.setItem("token", response.token);
    localStorage.setItem("timeLogin", new Date().getTime());

    // Chuyển hướng sau đăng nhập
    window.location.href = "/dashboard-default";
  } catch (error) {
    console.error("Login failed:", error.response?.data || error.message);
    alert("Đăng nhập thất bại. Vui lòng kiểm tra email hoặc mật khẩu.");
  }
};
</script>