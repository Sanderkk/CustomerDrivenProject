import React from "react";
import { BiX } from "react-icons/bi";

// Custom modal component
const Modal = ({ handleClose, show, children }) => {
  const showHideClassName = show ? "modal display-block" : "modal display-none";

  return (
    <div className={showHideClassName}>
      <section className="modal-main">
        <BiX onClick={handleClose} />
        {children}
      </section>
    </div>
  );
};

export default Modal;
