import axios from "axios";

export const http = axios.create({
  baseURL: import.meta.env.VITE_API_URL ?? "http://localhost:8080",
});

http.interceptors.response.use(
  (res) => res,
  (error) => Promise.reject(error)
);