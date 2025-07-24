import { Outlet } from "react-router";
import Header from "../components/header";
import Footer from "../components/footer";
import { ToastContainer } from "react-toastify";

export default function RootLayout() {
    return (
        <>
            <Header />
            <Outlet />
            <Footer />
            <ToastContainer aria-label="Bildirimler" />
        </>
    );
}
