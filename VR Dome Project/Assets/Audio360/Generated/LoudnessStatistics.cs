//------------------------------------------------------------------------------
// <auto-generated />
//
// This file was automatically generated by SWIG (http://www.swig.org).
// Version 3.0.12
//
// Do not make changes to this file unless you know what you are doing--modify
// the SWIG interface file instead.
//------------------------------------------------------------------------------

namespace TBE {

public class LoudnessStatistics : global::System.IDisposable {
  private global::System.Runtime.InteropServices.HandleRef swigCPtr;
  protected bool swigCMemOwn;

  internal LoudnessStatistics(global::System.IntPtr cPtr, bool cMemoryOwn) {
    swigCMemOwn = cMemoryOwn;
    swigCPtr = new global::System.Runtime.InteropServices.HandleRef(this, cPtr);
  }

  internal static global::System.Runtime.InteropServices.HandleRef getCPtr(LoudnessStatistics obj) {
    return (obj == null) ? new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero) : obj.swigCPtr;
  }

  ~LoudnessStatistics() {
    Dispose();
  }

  public virtual void Dispose() {
    lock(this) {
      if (swigCPtr.Handle != global::System.IntPtr.Zero) {
        if (swigCMemOwn) {
          swigCMemOwn = false;
          Audio360CSharpPINVOKE.delete_LoudnessStatistics(swigCPtr);
        }
        swigCPtr = new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero);
      }
      global::System.GC.SuppressFinalize(this);
    }
  }

  public float integrated {
    set {
      Audio360CSharpPINVOKE.LoudnessStatistics_integrated_set(swigCPtr, value);
    } 
    get {
      float ret = Audio360CSharpPINVOKE.LoudnessStatistics_integrated_get(swigCPtr);
      return ret;
    } 
  }

  public float shortTerm {
    set {
      Audio360CSharpPINVOKE.LoudnessStatistics_shortTerm_set(swigCPtr, value);
    } 
    get {
      float ret = Audio360CSharpPINVOKE.LoudnessStatistics_shortTerm_get(swigCPtr);
      return ret;
    } 
  }

  public float momentary {
    set {
      Audio360CSharpPINVOKE.LoudnessStatistics_momentary_set(swigCPtr, value);
    } 
    get {
      float ret = Audio360CSharpPINVOKE.LoudnessStatistics_momentary_get(swigCPtr);
      return ret;
    } 
  }

  public float truePeak {
    set {
      Audio360CSharpPINVOKE.LoudnessStatistics_truePeak_set(swigCPtr, value);
    } 
    get {
      float ret = Audio360CSharpPINVOKE.LoudnessStatistics_truePeak_get(swigCPtr);
      return ret;
    } 
  }

  public LoudnessStatistics() : this(Audio360CSharpPINVOKE.new_LoudnessStatistics(), true) {
  }

}

}
